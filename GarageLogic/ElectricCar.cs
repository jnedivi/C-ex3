﻿﻿﻿using System;
using System.Text;
using System.Collections.Generic;

namespace GarageLogic
{
    public class ElectricCar : Car
    {

        /*** Data Members ***/

        const float k_MaxBatteryLifeCar = 2.5f;

		/*** Class Logic ***/

		public ElectricCar(string i_LicenceNumber, string i_OwnerName, string i_OwnerPhoneNumber, string i_ModelName, List<Wheel> i_Wheels)
			: base(i_LicenceNumber, i_OwnerName, i_OwnerPhoneNumber, i_ModelName, i_Wheels)
        {
            Engine = new ElectricBasedEngine(0.0f, k_MaxBatteryLifeCar);
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();

            output.Append(base.ToString());
            output.Append(Engine.ToString());

            return output.ToString();
        }
    }
}
