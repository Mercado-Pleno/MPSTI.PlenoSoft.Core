using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MPSTI.PlenoSoft.Core.CrossProject.Results
{
	public class Result<TValue>
	{
		[JsonProperty("isOk")]
		public bool IsOk { get; private set; }

		[JsonProperty("value")]
		public TValue Value { get; private set; }

		[JsonProperty("messages")]
		public List<Message> Messages { get; private set; }

		internal Result(TValue value, bool isOk)
		{
			Messages = new List<Message>();
			Value = value;
			IsOk = isOk;
		}

		public Result<TValue> Add(Exception exception)
		{
			IsOk = false;
			return Add(new Message { Content = exception.Message, Type = MessageType.Error, Action = MessageAction.Show });
		}

		public Result<TValue> Add(string content, MessageType type = MessageType.Information, MessageAction action = MessageAction.None)
		{
			return Add(new Message { Content = content, Type = type, Action = action });
		}

		public Result<TValue> Add(Message message)
		{
			Messages.Add(message);
			return this;
		}
	}

	public static class Result
	{
		public static Result<string> Ok() => new Result<string>("OK", true);


		public static Result<TValue> Ok<TValue>(TValue value) => new Result<TValue>(value, true);


		public static Result<TValue> ToOk<TValue>(this TValue value) => new Result<TValue>(value, true);


		public static Result<TValue> Validate<TValue>(TValue value) => new Result<TValue>(value, false);


		public static Result<TValue> Error<TValue>(TValue value) => new Result<TValue>(value, false);


		public static Result<TValue> ToError<TValue>(this TValue value) => new Result<TValue>(value, false);
	}
}