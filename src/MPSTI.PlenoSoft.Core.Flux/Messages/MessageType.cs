﻿using System;
using System.Runtime.Serialization;

namespace MPSC.PlenoSoft.ControlFlux.Messages
{
	[Serializable, DataContract, Flags]
	public enum MessageType
	{
		Information = 1,
		Warning = 2,
		Validation = 4,
		Exception = 8,
		System = 16,
		Track = 32,
	}
}