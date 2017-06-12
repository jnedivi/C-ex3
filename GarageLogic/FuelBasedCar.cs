﻿﻿﻿using System;
using System.Text;
using System.Collections.Generic;
            

namespace GarageLogic
{
    public class FuelBasedCar : Car
    {

        /*** Data Members ***/
        private const FuelBasedEngine.eFuelType k_FuelTypeForCar = FuelBasedEngine.eFuelType.Octane98;
        private const float k_MaxAmountOfFuelForCar = 42.0f;

		/*** Getters and Setters ***/


		/*** Class Logic ***/

		public FuelBasedCar(string i_LicenceNumber, string i_OwnerName, string i_OwnerPhoneNumber, string i_ModelName, List<Vehicle.Wheel> i_Wheels)
			: base(i_LicenceNumber, i_OwnerName, i_OwnerPhoneNumber, i_ModelName, i_Wheels)
        {
            Engine = new FuelBasedEngine(0.0f, k_MaxAmountOfFuelForCar, k_FuelTypeForCar);
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
