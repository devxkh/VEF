/*
Toothrot .Net control library 
Copyright (C) 2008 Pau Novau Lebrato

This library is free software; you can redistribute it and/or modify it under 
the terms of the GNU Lesser General Public License as published by the Free Software 
Foundation; either version 2.1 of the License, or (at your option) any later 
version.

This program is distributed in the hope that it will be useful, but WITHOUT 
ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with 
this program; if not, write to the Free Software Foundation, Inc., 59 Temple 
Place - Suite 330, Boston, MA 02111-1307, USA
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Toothrot.Action
{
	public class History
	{
		/// List of actions that can be undone.
		private List< IAction > m_undoableActions;

		/// List of actions that can be redone.
		private List< IAction > m_redoableActions;

		/// Event called when the history changes.
		/// This means that actions were added or removed from the undoable
		/// and redoable action lists.
		public event EventHandler Changed;

		/// Event called when the history is cleared.
		public event EventHandler Cleared;

		/// Event called when an action is executed.
		public event EventHandler< ActionEventArgs > ActionExecuted;

		/// Event called when an action is undone.
		public event EventHandler< ActionEventArgs > ActionUndone;

		/// Event called when an action is redone.
		public event EventHandler< ActionEventArgs > ActionRedone;

		/// Check how many actions can be undone. It doesn't relate 1:1 to the number
		/// of undo steps, as some actions may cause more than one action to be undone.
		public int UndoableActionCount
		{
			get { return m_undoableActions.Count; }
		}

		/// Check how many actions can be redone. It doesn't relate 1:1 to the number
		/// of redo steps, as some actions may cause more than one action to be redone.
		public int RedoableActionCount
		{
			get { return m_redoableActions.Count; }
		}

		public History()
		{
			m_undoableActions = new List< IAction >();
			m_redoableActions = new List< IAction >();
		}



		/**
		 * Clears the history of any undoable and redoable events.
		 * 
		 * Calls the Cleared and Changed events.
		 */
		public void Clear()
		{
			if ( m_undoableActions.Count == 0 && m_redoableActions.Count == 0 )
			{
				return;
			}

			m_undoableActions.Clear();
			m_redoableActions.Clear();

			OnClear();
			OnChange();
		}



		/**
		 * Execute an action on this History.
		 * 
		 * Normally the history will be updated so that the action can be undone
		 * or redone. The exact way how the action affects the history depends
		 * on its HistoryOperation.
		 * 
		 * If the action has been executed already, this operation will fail.
		 * 
		 * When an action is executed successfully, the redoable list of actions
		 * is cleared.
		 * 
		 * Events Cleared, Changed and ActionExecuted may result from this call.
		 * 
		 * @param action Action to execute in this History instance. If a null
		 *		value is passed, the function will return FAILURE.
		 * 
		 * @return Result from executing the action.
		 */ 
		public ActionResult ExecuteAction( IAction action )
		{
			if ( action == null )
			{
				return ActionResult.FAILURE;
			}

			if ( action.Executed )
			{
				return ActionResult.FAILURE;
			}

			ActionResult result = action.Execute();

			if ( result == ActionResult.SUCCESS )
			{
				m_redoableActions.Clear();

				if ( action.HasHistoryOperation( HistoryOperation.CLEAR_ON_SUCCESS ) )
				{
					// We clear before than checking store on success, so that they
					// aren't mutually exclusive.
					Clear();
				}

				if ( action.HasHistoryOperation( HistoryOperation.STORE_ON_SUCCESS ) )
				{
					m_undoableActions.Add( action );
					OnChange();
				}
			}

			OnActionExecuted( action, result );

			return result;
		}



		/**
		 * Undo the last action stored by this History.
		 * 
		 * If the action has the PASS_THROUGH flag set, the next
		 * action will try to be undone too ( recusively ).
		 * 
		 * Events Changed and ActionUndone may result from this operation.
		 * 
		 * @return Result from undoing the action.
		 */
		public ActionResult UndoAction()
		{
			if ( ! CanUndoAction() )
			{
				return ActionResult.FAILURE;
			}

			IAction action = GetActionToUndo();

			ActionResult result = action.Undo();

			if ( result == ActionResult.SUCCESS )
			{
				RemoveLast( m_undoableActions );
				m_redoableActions.Add( action );
			}
			else
			{
				m_undoableActions.Clear();
			}

			OnChange();
			OnActionUndone( action, result );

			// We check PASS_THROUGH at the end because we want
			// events called individually for each action undone.
			if ( result == ActionResult.SUCCESS )
			{
				if ( action.HasHistoryOperation( HistoryOperation.PASS_THROUGH ) )
				{
					if ( CanUndoAction() )
					{
						result = UndoAction();
					}
				}
			}

			return result;
		}



		/**
		 * Redo the last action undone in this history.
		 * 
		 * If the action has the PASS_THROUGH flag set, the next
		 * action will try to be redone too ( recursively ).
		 * 
		 * Events Changed, Cleared and ActionRedone may result from this operation.
		 * 
		 * @return Result from executing the action.
		 */
		public ActionResult RedoAction()
		{
			if ( ! CanRedoAction() )
			{
				return ActionResult.FAILURE;
			}

			IAction action = GetActionToRedo();

			ActionResult result = action.Execute();

			if ( result == ActionResult.SUCCESS )
			{
				RemoveLast( m_redoableActions );

				if ( action.HasHistoryOperation( HistoryOperation.STORE_ON_SUCCESS ) )
				{
					m_undoableActions.Add( action );
				}

				if ( action.HasHistoryOperation( HistoryOperation.CLEAR_ON_SUCCESS ) )
				{
					Clear();
				}
			}
			else
			{
				m_redoableActions.Clear();
			}

			OnChange();
			OnActionRedone( action, result );

			// We check PASS_THROUGH at the end because we want
			// events called individually for each action redone.
			if ( result == ActionResult.SUCCESS )
			{
				if ( action.HasHistoryOperation( HistoryOperation.PASS_THROUGH ) )
				{
					if ( CanRedoAction() )
					{
						result = RedoAction();
					}
				}
			}

			return result;
		}



		/**
		 * Checks if there are actions that can be undone.
		 * 
		 * @return True if there are actions to undo, false otherwise.
		 */ 
		public bool CanUndoAction()
		{
			return ( 0 < m_undoableActions.Count );
		}



		/**
		 * Checks if there are actions that can be redone.
		 * 
		 * @return True if there are actions to redo, false otherwise.
		 */
		public bool CanRedoAction()
		{
			return ( 0 < m_redoableActions.Count );
		}




		/**
		 * Obtain a list that contains the names of the actions to undo.
		 * 
		 * @return List with the names of the actions to undo.
		 */ 
		public List< String > GetUndoableActionNames()
		{
			return GetActionNames( m_undoableActions );
		}




		/**
		 * Obtain a list that contains the names of the actions to redo.
		 * 
		 * @return List with the names of the actions to redo.
		 */ 
		public List< String > GetRedoableActionNames()
		{
			return GetActionNames( m_redoableActions );
		}



		/**
		 * Obtain a list of names from an action list.
		 * 
		 * @return List with the names of the actions in the list.
		 */ 
		private List< String > GetActionNames( List< IAction > actionList )
		{
			List< String > actionNames = new List< String >();

			foreach ( IAction action in actionList )
			{
				actionNames.Add( action.Name );
			}

			return actionNames;
		}



		/**
		 * Obtain the next action that should be undone.
		 * 
		 * @return Action to be undone.
		 */ 
		private IAction GetActionToUndo()
		{
			return GetLastAction( m_undoableActions );
		}



		/**
		 * Obtain the next action that should be redone.
		 * 
		 * @return Action to be redone.
		 */ 
		private IAction GetActionToRedo()
		{
			return GetLastAction( m_redoableActions );
		}



		/**
		 * Get the last action on an action list.
		 * 
		 * The list must not be empty.
		 * 
		 * @param actionList List to get the action from.
		 * 
		 * @return Last Action in an action list.
		 */ 
		private IAction GetLastAction( List< IAction > actionList )
		{
			Debug.Assert( 0 < actionList.Count );

			return actionList[ actionList.Count - 1 ];
		}



		/**
		 * Remove the last action from an action list.
		 * 
		 * @param actionList List to remove the action from.
		 */ 
		private void RemoveLast( List< IAction > actionList )
		{
			if ( actionList.Count == 0 )
			{
				return;
			}

			actionList.RemoveAt( actionList.Count - 1 );
		}



		#region event signaling


		private void OnChange()
		{
			if ( Changed != null )
			{
				Changed( this, new EventArgs() );
			}
		}

		private void OnClear()
		{
			if ( Cleared != null )
			{
				Cleared( this, new EventArgs() );
			}
		}

		private void OnActionExecuted( IAction action, ActionResult result )
		{
			if ( ActionExecuted != null )
			{
				ActionExecuted( this, new ActionEventArgs( action, result ) );
			}
		}

		private void OnActionUndone( IAction action, ActionResult result )
		{
			if ( ActionUndone != null )
			{
				ActionUndone( this, new ActionEventArgs( action, result ) );
			}
		}

		private void OnActionRedone( IAction action, ActionResult result )
		{
			if ( ActionRedone != null )
			{
				ActionRedone( this, new ActionEventArgs( action, result ) );
			}
		}

		#endregion event signaling
	}
}
