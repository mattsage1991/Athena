﻿namespace Athena.BuildingBlocks.Domain.Exceptions
{
    public class BusinessRuleValidationException : DomainException
    {
        public IBusinessRule BrokenRule { get; }

        public string Details { get; }

        public BusinessRuleValidationException(IBusinessRule brokenRule) : base(brokenRule.Message)
        {
            BrokenRule = brokenRule;
            this.Details = brokenRule.Message;
        }

        public override string ToString()
        {
            return $"{BrokenRule.Message}";
        }
    }
}