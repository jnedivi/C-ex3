﻿using System;
using System.Collections.Generic;

namespace GarageLogic
{
    public class ValueOutOfRangeException : Exception
    {
        private float m_MinimumValue;
        private float m_MaximumValue;

        public float MinValue
        {
            get { return this.m_MinimumValue; }
            set { this.m_MinimumValue = value; }
        }

        public float MaxVlaue
        {
            get { return this.m_MaximumValue; }
            set { this.m_MaximumValue = value; }
        }

        public ValueOutOfRangeException(string i_OutputMessage, float i_MinVlaue, float i_MaxValue) : base(i_OutputMessage)
        {
            MaxVlaue = i_MaxValue;
            MinValue = i_MinVlaue;
        }

        public override string ToString()
        {
            return string.Format("Value must be between {0} and {1}, please enter a value in this range.", MinValue, MaxVlaue);
        }
    }
}
