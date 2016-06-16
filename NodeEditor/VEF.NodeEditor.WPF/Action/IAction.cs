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
	public interface IAction
	{
		/// Event triggered when the action has been executed.
		event EventHandler< ActionEventArgs > ExecuteEvent;

		
		/// Event triggered when the action has been undone.
		event EventHandler< ActionEventArgs > UndoEvent;

		/// Event triggered when the action has been redone.
		event EventHandler< ActionEventArgs > RedoEvent;

		/// Descriptive name of the action.
		String Name
		{
			get;
		}

		/// Effect of the action on the action history.
		HistoryOperation HistoryOperation
		{
			get;
		}

		/// string containing a description of the latest error.
		String FailureReason
		{
			get;
		}

		/// Check if the action has been executed.
		bool Executed
		{
			get;
		}


		/**
		 * Execute this action.
		 * 
		 * In case of failure, the string FailureReason should indicate the reason.
		 * 
		 * It will call ExecuteEvent or RedoEvent, depending on the circumstances.
		 * 
		 * @return Result of the operation indicating if it was successful or not.
		 */
		ActionResult Execute();

		/**
		 * Undo this action
		 * 
		 * In case of failure, the string FailureReason should indicate the reason.
		 * 
		 * If successful it will call UndoEvent.
		 * 
		 * @return Result of the operation indicating if it was successful or not.
		 */
		ActionResult Undo();


		/**
		 * Query history operation combination is active.
		 * 
		 * All the flags queried in a single call need to be active
		 * for the function to return true.
		 * 
		 * @param historyOperationFlags Contains the flags to query.
		 * 
		 * @return True if action has all of the flags in historyOperationFlags
		 *		active, false otherwise.
		 */ 
		bool HasHistoryOperation( HistoryOperation historyOperationFlag );
	}
}
