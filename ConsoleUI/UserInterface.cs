﻿﻿﻿﻿﻿﻿﻿using GarageLogic;
using System;
using System.Text;
using System.Collections.Generic;
namespace ConsoleUI
{
    public class UserInterface
    {
        /*** Data Members ***/

        private Garage m_Garage;
        private const int k_NumberOfMainMenuItems = 8;

        /*** Constructor ***/

        public UserInterface()
        {
            m_Garage = new Garage();
            mainMenu();
        }

        /*** Class Logic ***/

        private void mainMenu()
        {

            System.Console.WriteLine(MainMenuMessage());
            int mainMenuInputNumber = this.promptUserForMenuSelection(k_NumberOfMainMenuItems);

            if (mainMenuInputNumber == k_NumberOfMainMenuItems)
            {
                System.Console.WriteLine("Exit program selected. Bye Bye ...");
                System.Environment.Exit(1);
            }

            switch (mainMenuInputNumber)
            {
                case 1:
                    /* 1) Insert new Vehicle into Garage. */
                    insertNewVehicleIntoGarage();
                    break;
                case 2:
                    /* 2) Display list of licence numbers. */
                    displayListOfLicenceNumbers();
                    break;
                default:
                    /* Options 3 - 7 */
                    menuOptions(mainMenuInputNumber);
                    break;
            }
        }

        /* 1) Insert new Vehicle into Garage */
        private void insertNewVehicleIntoGarage()
        {
            System.Console.WriteLine(createMenuStringFromEnum(typeof(Factory.eVehicleType), "Insert New Vehicle"));
            int vehicleTypeNumber = promptUserForMenuSelection(Enum.GetNames(typeof(Factory.eVehicleType)).Length);
            Factory.eVehicleType vehicleToAdd = (Factory.eVehicleType)vehicleTypeNumber - 1;
            string licenseNumber;
            this.promptUserForLicenseNumber(out licenseNumber);

            if (this.m_Garage.IsInGarage(licenseNumber))
            {
                System.Console.WriteLine("Veicle currently in garage. Status changed to: In Repair.");
                this.m_Garage.StatusInRepairedUpdate(licenseNumber);
            }
            else
            {
                System.Console.WriteLine("Please enter Owner name:");
                string ownerName = System.Console.ReadLine();
                System.Console.WriteLine("Please enter Owners phone number:");
                string ownerPhoneNumber = System.Console.ReadLine();
                while (!Vehicle.isLegalPhoneNumber(ownerPhoneNumber))
                {
                    System.Console.WriteLine("Please enter valid phone number:");
                    ownerPhoneNumber = System.Console.ReadLine();
                }

                System.Console.WriteLine("Please enter Vehicles Model Name:");
                string vehicleModelName = System.Console.ReadLine();

                this.m_Garage.InsertNewVehicle(vehicleToAdd, licenseNumber, ownerName, ownerPhoneNumber, vehicleModelName);

                System.Console.WriteLine(createMenuStringFromEnum(typeof(Vehicle.eTireAirPressureStatus), "Do all tires have the same air pressure"));
                int tireStatusNumber = promptUserForMenuSelection(Enum.GetNames(typeof(Factory.eVehicleType)).Length);
                Vehicle createdVehicle;
                m_Garage.GetVehicle(licenseNumber, out createdVehicle);
                List<float> tirePressures = new List<float>();

                if (tireStatusNumber == 1)
                {
                    float singleTireAirPressure = this.getTirePressureFromUser(createdVehicle);
                    for (int i = 0; i < createdVehicle.NumberOfWheels; i++)
                    {
                        tirePressures.Add(singleTireAirPressure);
                    }
                }
                else
                {
                    for (int i = 0; i < createdVehicle.NumberOfWheels; i++)
                    {
                        tirePressures.Add(getTirePressureFromUser(createdVehicle));
                    }
                }

                System.Console.WriteLine("Please enter Tires Manufacturer Name:");
                string tiresManufacturerName = System.Console.ReadLine();

                Factory.CreateWheels(createdVehicle, tiresManufacturerName, tirePressures); //TODO 
                int userChoice;

                if (createdVehicle is Car)
                {

                    System.Console.WriteLine(createMenuStringFromEnum(typeof(Car.eColor), "Enter the car's color:"));
                    userChoice = promptUserForMenuSelection(Enum.GetNames(typeof(Car.eColor)).Length);
                    ((Car)createdVehicle).Color = (Car.eColor)userChoice - 1;

                    System.Console.WriteLine(createMenuStringFromEnum(typeof(Car.eNumOfDoors), "Enter the car's number of doors:"));
                    userChoice = promptUserForMenuSelection(Enum.GetNames(typeof(Car.eNumOfDoors)).Length);
                    ((Car)createdVehicle).NumberOfDoors = (Car.eNumOfDoors)userChoice - 1;

                    Factory.SetCarDoorsAndColor((Car)createdVehicle, (Car.eNumOfDoors)userChoice, (Car.eColor)userChoice); //TODO
                }
                else if (createdVehicle is Motorcycle)
                {
                    System.Console.WriteLine(createMenuStringFromEnum(typeof(Motorcycle.eLicenseType), "Enter the motorcycles's license type:"));
                    userChoice = promptUserForMenuSelection(Enum.GetNames(typeof(Motorcycle.eLicenseType)).Length);
                    ((Motorcycle)createdVehicle).LicenceType = (Motorcycle.eLicenseType)userChoice - 1;
                    System.Console.WriteLine("Please enter engine volume:");
                    userChoice = (int)getFloatFromUser(int.MaxValue, 0);
                    ((Motorcycle)createdVehicle).EngineVolume = userChoice;

                }
                else if (createdVehicle is Truck)
                {

                    System.Console.WriteLine(createMenuStringFromEnum(typeof(Truck.eIsCarryingHazardousMaterials), "Is the truck carrying hazardous materials?:"));
                    userChoice = promptUserForMenuSelection(Enum.GetNames(typeof(Truck.eIsCarryingHazardousMaterials)).Length);

                    if (userChoice == 0)
                    {
                        ((Truck)createdVehicle).HasHazardousMaterials = true;
                    }
                    else
                    {
                        ((Truck)createdVehicle).HasHazardousMaterials = false;
                    }

                    System.Console.WriteLine("Please enter maximum allowed weight:");
                    float maxWeightAllowed = getFloatFromUser(int.MaxValue, 0);
                    ((Truck)createdVehicle).MaxWeightAllowed = maxWeightAllowed;
                }

                if (createdVehicle.Engine is FuelBasedEngine)
                {
					System.Console.WriteLine("Please enter current amount of fuel:");
                    float currentAmountOfFuel = getFloatFromUser( (int)((FuelBasedEngine)createdVehicle.Engine).MaxAmountOfFuel , 0);
                    ((FuelBasedEngine)createdVehicle.Engine).CurrentAmountOfFuel = currentAmountOfFuel;
                }
                else if (createdVehicle.Engine is ElectricBasedEngine)
                {
					System.Console.WriteLine("Please enter remaining time of engine operation in hours:");
                    float currentBatteryEnergy = getFloatFromUser((int)((ElectricBasedEngine)createdVehicle.Engine).MaxBatteryLife, 0);
                    ((ElectricBasedEngine)createdVehicle.Engine).RemainingTimeOnBattery = currentBatteryEnergy;
                }
            }

            mainMenu();
        }


        /* 2) Display list of licence numbers */
        private void displayListOfLicenceNumbers()
        {
            //TODO: Give user option if to filter or not
            System.Console.Clear();

            System.Console.WriteLine(createMenuStringFromEnum(typeof(Vehicle.eVehicleStatus), "Enter a Vehicle type"));
            int userChoice = promptUserForMenuSelection(Enum.GetNames(typeof(Vehicle.eVehicleStatus)).Length);
            Dictionary<string, Vehicle>.KeyCollection list = m_Garage.GetFilteredListOfLicenseNumbers((Vehicle.eVehicleStatus)(userChoice - 1));
            int vehicleNumber = 1;
            foreach (string license in list)
            {

                System.Console.WriteLine(string.Format("{0}. Licence number: {1}{2}", vehicleNumber, license, Environment.NewLine));
                vehicleNumber++;
            }

            if (list.Count == 0)
            {
                System.Console.WriteLine("No Vehicles Available" + Environment.NewLine);
            }

            System.Console.Clear();
            mainMenu();
        }

        /*** Options 3 - 7 ***/
        private void menuOptions(int i_MainMenuSelection)
        {
            int userChoice;
            System.Console.Clear();
            string licenseNumber;
            Vehicle currentVehicle = this.promptUserForLicenseNumber(out licenseNumber);
            if (currentVehicle != null)
            {
                switch (i_MainMenuSelection)
                {
                    case 3:
                        /* 3) Change a vehicle status */
                        // TODO: add catches
                        System.Console.WriteLine(createMenuStringFromEnum(typeof(Vehicle.eVehicleStatus), "Enter a Vehicle Status"));
                        userChoice = promptUserForMenuSelection(Enum.GetNames(typeof(Vehicle.eVehicleStatus)).Length);
                        try
                        {
                            m_Garage.ChangeVehicleStatus(licenseNumber, (Vehicle.eVehicleStatus)(userChoice - 1));
                        }
                        catch(ArgumentException ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        break;
                    case 4:
                        /* 4) Inflate tires to max */
                        currentVehicle.InflateAllWheelsToMax();
                        break;
                    case 5:
                        /* 5) Refuel vehicle */
                        // TODO: get function to work, as well as the catches
                        System.Console.WriteLine(createMenuStringFromEnum(typeof(FuelBasedEngine.eFuelType), "Enter a Fuel Type"));
                        userChoice = this.promptUserForMenuSelection(Enum.GetNames(typeof(FuelBasedEngine.eFuelType)).Length);
                        System.Console.WriteLine("Please enter amount to refuel");
                        float amountToRefuel = this.getFloatFromUser(0, int.MaxValue);
                        try
                        {
                            this.m_Garage.RefuelVehicle(licenseNumber, (FuelBasedEngine.eFuelType)(userChoice - 1), amountToRefuel);
                        }
                        catch (FormatException ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        catch (ValueOutOfRangeException ex)
                        {
                            Console.WriteLine(ex.ToString());

                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                        break;
                    case 6:
                        /* 6) charge vehicle */
                        System.Console.WriteLine("Please enter amount to recharge:");
                        float amountToRecharge;
                        try
                        {
                            amountToRecharge = this.getFloatFromUser(0, int.MaxValue);
                            m_Garage.ChargeElectricVehice(licenseNumber, amountToRecharge);
                        }
                        catch (Exception e)
                        {
                            System.Console.WriteLine("we caught a exception");
                        }
                        /* 6) charge vehicle */
                        // TODO: get function to work, add catches
                        break;
                    case 7:
                        /* 7) Display vehicle information */
                        System.Console.WriteLine(currentVehicle.ToString());
                        break;
                }
            }
            else
            {
                System.Console.WriteLine(string.Format("Vehicle with licence number {0} is not in the garage.{1}", licenseNumber, Environment.NewLine));
            }

            mainMenu();
        }

        private float getFloatFromUser(int i_MaxNumber, int i_MinNumber)
        {
            float userInputNumber;
            string input = System.Console.ReadLine();
            while (!(float.TryParse(input, out userInputNumber) && userInputNumber >= i_MinNumber && userInputNumber <= i_MaxNumber))
            {
                System.Console.Write(string.Format("Invalid Input. Please eneter a number between {0} and {1}.", i_MinNumber, i_MaxNumber)); //TODO: exception trown 
                input = System.Console.ReadLine();
            }

            return userInputNumber;
        }

        /* Get Menu Selection From User */
        private int promptUserForMenuSelection(int i_NumberOfItems)
        {
            string userInputString = System.Console.ReadLine();
            int userInputNumber;
            string messageToUser = string.Format("Please enter a number between 1 and {0}", i_NumberOfItems);

            while (!(int.TryParse(userInputString, out userInputNumber) && userInputNumber >= 1 && userInputNumber <= i_NumberOfItems))
            {
                System.Console.WriteLine("Invalid Input. " + messageToUser);
                userInputString = System.Console.ReadLine();
            }
            System.Console.Clear();
            return userInputNumber;
        }

        /* Get Licence number and Vehilce from user */
        private Vehicle promptUserForLicenseNumber(out string o_licenceNumber)
        {
            System.Console.WriteLine("Please enter the licence number of your vehicle or Q to cancel:");
            o_licenceNumber = System.Console.ReadLine();
            if (o_licenceNumber == "Q" || o_licenceNumber == "q")
            {
                mainMenu();
            }
            else
            {
                while (!Vehicle.isLegalLicenseNumber(o_licenceNumber))
                {
                    System.Console.WriteLine("Invalid input. please enter a legal licence plate number.");
                    o_licenceNumber = System.Console.ReadLine();
                }
            }

            Vehicle currentVehicle;

            return m_Garage.GetVehicle(o_licenceNumber, out currentVehicle) ? currentVehicle : null;
        }

        /* Get Tire pressures from user */
        private float getTirePressureFromUser(Vehicle i_Vehicle)
        {
            float tirePressure;
            string maxAirPressumeMessage = string.Format("Please enter a number below or equal to the max air pressure: {0}.", i_Vehicle.MaxAirPressure);

            System.Console.WriteLine(maxAirPressumeMessage);
            string userInputPressure = System.Console.ReadLine();

            while (!(float.TryParse(userInputPressure, out tirePressure) && tirePressure <= i_Vehicle.MaxAirPressure && tirePressure >= 0))
            {
                System.Console.WriteLine(string.Format("Invalid Input. {0}", maxAirPressumeMessage));
                userInputPressure = System.Console.ReadLine();
            }

            return tirePressure;
        }


        private static string createMenuStringFromEnum(Type i_EnumType, string i_Title)
        {
            int menuNumber = 1;
            StringBuilder menuString = new StringBuilder();
            menuString.AppendLine(i_Title);

            foreach (string menuValue in Enum.GetNames(i_EnumType))
            {
                menuString.Append(menuNumber.ToString()).Append(". ");
                menuString.Append(menuValue).Append(Environment.NewLine);
                menuNumber++;
            }

            return menuString.ToString();
        }

        /*private static void refuelVehicle(ref Vehicle io_Vehicle)
        {

        }*/



        /*** End of class ***/


        public static string MainMenuMessage()
        {

            string mainMenuMessage = string.Format(
                @"Main Menu
Please Select a task number you wish to complete:
1) Insert new Vehicle into Garage.
2) Display list of licence numbers.
3) Change a Vehicle's status
4) Inflate tires
5) Refuel a vehicle.
6) Charge a electric vehice.
7) Display vehicle information.
8) Quit ");

            return mainMenuMessage;
        }
    }


}


