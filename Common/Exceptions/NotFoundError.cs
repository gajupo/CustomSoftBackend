﻿using FluentResults;

namespace Common.Exceptions
{
    public class NotFoundError: Error
    {
        public NotFoundError(string message)
            :base(message)
        {
            
        }
    }
}
