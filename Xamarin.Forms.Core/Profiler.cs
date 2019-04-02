using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Internal
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public struct Profile : IDisposable
	{
		const int Capacity = 1000;

		[DebuggerDisplay("{Name,nq} {Id} {Ticks}")]
		public struct Datum
		{
			public string Name;
			public string Id;
			public long Ticks;
			public int Depth;
			public int Line;
		}
		public static List<Datum> Data = new List<Datum>(Capacity);

		static Stack<Profile> _stack = new Stack<Profile>(Capacity);
		static int _depth = 0;
		static bool _running = false;
		static Stopwatch _stopwatch = new Stopwatch();

		readonly long start;
		readonly string name;
		readonly int slot;

		public static void Start() 
		{
			_running = true;
		}

		public static void Stop()
		{
			// unwind stack
			_running = false;
			while (_stack.Count > 0)
				_stack.Pop();
		}

		public static void FrameBegin(
			[CallerMemberName] string name = "",
			string id = null,
			[CallerLineNumber] int line = 0)
		{
			if (!_running)
				return;

			FrameBeginBody(name, id, line);
		}

		public static void FrameEnd()
		{
			if (!_running)
				return;

			FrameEndBody();
		}

		public static void FramePartition(
			string id,
			[CallerLineNumber] int line = 0)
		{
			if (!_running)
				return;

			FramePartitionBody(id, line);
		}

		static void FrameBeginBody(
			string name,
			string id,
			int line)
		{
			if (!_stopwatch.IsRunning)
				_stopwatch.Start();

			_stack.Push(new Profile(name, id, line));
		}

		static void FrameEndBody()
		{
			var profile = _stack.Pop();
			profile.Dispose();
		}

		static void FramePartitionBody(
			string id, 
			int line)
		{
			var profile = _stack.Pop();
			var name = profile.name;
			profile.Dispose();

			FrameBegin(name, id, line);
		}

		Profile(
			string name,
			string id,
			int line)
		{
			this = default(Profile);
			this.start = _stopwatch.ElapsedTicks;

			this.name = name;

			this.slot = Data.Count;
			Data.Add(new Datum()
			{
				Depth = _depth,
				Name = name,
				Id = id,
				Ticks = -1,
				Line = line
			});

			_depth++;
		}

		public void Dispose()
		{
			if (_running && this.start == 0)
				return;

			var ticks = _stopwatch.ElapsedTicks - this.start;
			--_depth;

			var datum = Data[slot];
			datum.Ticks = ticks;
			Data[this.slot] = datum;
		}
	}
}