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

namespace Toothrot.Action
{

	/**
	 * Indicates the effect of an action has on the action history.
	 */
	[Flags]
	public enum HistoryOperation
	{
		/// The action isn't saved in the action history when executed.
		NONE = 0,
		
		/// The action is stored in the action history when executed successfully.
		STORE_ON_SUCCESS = 1,

		/// The action clears the previous action history when executed successfully.
		CLEAR_ON_SUCCESS = 2,

		/// When undone or redone, the next action on the action history will be undone or redone too.
		PASS_THROUGH = 4
	}
}