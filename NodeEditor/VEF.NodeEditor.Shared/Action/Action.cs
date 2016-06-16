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

namespace Toothrot.Action
{
	abstract public class Action : IAction
	{
		String m_name;
		HistoryOperation m_historyOperation;
		String m_failureReason;
		bool m_executedSuccessfully;
		bool m_executed;

		public event EventHandler< ActionEventArgs > ExecuteEvent;
		public event EventHandler< ActionEventArgs > UndoEvent;
		public event EventHandler< ActionEventArgs > RedoEvent;

		public String Name
		{
			get { return m_name; }
		}

		public HistoryOperation HistoryOperation
		{
			get { return m_historyOperation; }
		}

		public String FailureReason
		{
			get { return m_failureReason; }
			protected set { m_failureReason = value; }
		}

		public bool Executed
		{
			get { return m_executed; }
		}

		public Action( String name )
		{
			m_executed = false;
			m_name = name;

			m_historyOperation = HistoryOperation.NONE;
		}

		protected void AddHistoryOperation( HistoryOperation historyOperationFlags )
		{
			m_historyOperation |= historyOperationFlags;
		}

		protected void RemoveHistoryOperation( HistoryOperation historyOperationFlags )
		{
			m_historyOperation &= ~historyOperationFlags;
		}

		public bool HasHistoryOperation( HistoryOperation historyOperationFlags )
		{
			return ( ( m_historyOperation & historyOperationFlags ) == historyOperationFlags );
		}


		public ActionResult Execute()
		{
			if ( m_executedSuccessfully )
			{
				ActionResult result = OnRedo();

				if ( RedoEvent != null )
				{
					RedoEvent( this, new ActionEventArgs( this, result ) );
				}

				return result;
			}
			else
			{
				ActionResult result = OnExecute();
				
				m_executed = true;
				m_executedSuccessfully = ( result == ActionResult.SUCCESS );

				if ( ExecuteEvent != null )
				{
					ExecuteEvent( this, new ActionEventArgs( this, result ) );
				}

				return result;
			}
		}


		public ActionResult Undo()
		{
			ActionResult result = OnUndo();

			if ( UndoEvent != null )
			{
				UndoEvent( this, new ActionEventArgs( this, result ) );
			}

			return result;
		}

		abstract protected ActionResult OnExecute();

		virtual protected ActionResult OnUndo()
		{
			FailureReason = "Undo not supported for " + Name;
			return ActionResult.FAILURE;
		}

		virtual protected ActionResult OnRedo()
		{
			FailureReason = "Redo not supported for " + Name;
			return ActionResult.FAILURE;
		}

	}
}
