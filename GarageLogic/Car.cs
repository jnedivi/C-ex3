﻿using System;
namespace GarageLogic
{

	public class Car : Vehicle
	{

		/*** Data Members ***/
	
		private eColor m_Color;
        private eNumOfDoors m_NumberOfDoors;

        public Car()
        {
            this.NumberOfWheels = 4;
            this.Tire.MaxAirPressure = 30f;
            this.CreateTires(NumberOfWheels, this.Tire);
        }

		/*** Getters and Setters***/

		public eColor Color
        {
            get { return this.m_Color; }
            set { this.m_Color = value; }
        }

		public eNumOfDoors NumberOfDoors
		{
			get { return this.m_NumberOfDoors; }
			set { this.m_NumberOfDoors = value; }
		}



		public enum eColor
		{
			Yellow,
			White,
			Black,
			Blue,
		}

		public enum eNumOfDoors
		{
			Two,
			Three,
			Four,
			Five,
		}
	}
}
