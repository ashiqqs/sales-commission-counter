using SalePurchaseAccountant.DAL;
using SalePurchaseAccountant.Models.Employee;
using SalePurchaseAccountant.Models.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalePurchaseAccountant.BLL
{
    public class PolicyBll
    {
        private readonly SalesmanGetway _salesman;
        public PolicyBll()
        {
            _salesman = new SalesmanGetway();
        }
        public bool IsEligible(double salesAmount)
        {
            return salesAmount >= 4000;
        }
        public bool IsEligibleForOrdinalCommission(SalesmanModel salesman)
        {
            double personalSale = _salesman.GetSalesAmount(code: salesman.Code);
            if (!IsEligible(personalSale)){
                return false;
            }
            double associatesSale = _salesman.GetAssociatesSalesAmount(referenceCode: salesman.ReferenceCode);
            switch (salesman.Designation)
            {
                case Designation.C:{
                        return associatesSale >= 16000;
                    }
                case Designation.D:{
                       if(associatesSale >= 6000)
                        {
                            var associates = _salesman.GetAssociates(salesman.Code, (int)Designation.C);
                            bool isEligible = false;
                            foreach (var associate in associates)
                            {
                                if (isEligible) { return true; }
                                isEligible =  IsEligibleForOrdinalCommission(associate);
                            }
                        }
                        return false;
                    }
                case Designation.E:{
                        if ((personalSale + associatesSale) >= 70000)
                        {
                            var associatesB = _salesman.GetAssociates(salesman.Code, (int)Designation.B);
                            int eligibleB = 0;
                            foreach(var associate in associatesB)
                            {
                                if (eligibleB == 3) { break; }
                                if (IsEligible(_salesman.GetSalesAmount(code: associate.Code)))
                                {
                                    eligibleB++;
                                }
                            }
                            if (eligibleB == 3)
                            {
                                var associatesC = _salesman.GetAssociates(salesman.Code, (int)Designation.C);
                                int eligibleC = 0;
                                foreach (var associate in associatesC)
                                {
                                    if (eligibleC == 3) { break; }
                                    if (IsEligibleForOrdinalCommission(associate))
                                    {
                                        eligibleC++;
                                    }
                                }
                                return eligibleC == 3;
                            }
                        }
                        return false;
                    }
                case Designation.F:{
                        #region Checking B Eligiblity
                        var associatesB = _salesman.GetAssociates(salesman.Code, (int)Designation.B);
                        int eligibleB = 0;
                        foreach(var associate in associatesB)
                        {
                            if (eligibleB == 2) { break; }
                            if (IsEligible(_salesman.GetSalesAmount(code: associate.Code)))
                            {
                                eligibleB++;
                            }
                        }
                        if (eligibleB<2) { return false; }
                        #endregion

                        #region #region Checking C Eligiblity
                        var associatesC = _salesman.GetAssociates(salesman.Code, (int)Designation.C);
                        bool eligibleC = false;
                        foreach (var associate in associatesC)
                        {
                            if (eligibleC) { break; }
                            if (IsEligibleForOrdinalCommission(associate))
                            {
                                eligibleC = true;
                            }
                        }
                        if (!eligibleC) { return false; }
                        #endregion

                        #region #region Checking E Eligiblity
                        var associatesE = _salesman.GetAssociates(salesman.Code, (int)Designation.E);
                        int eligibleE = 0;
                        foreach (var associate in associatesE)
                        {
                            if (eligibleE == 2) { break; }
                            if (IsEligibleForOrdinalCommission(associate))
                            {
                                eligibleE++;
                            }
                        }
                        return eligibleE >= 2; 
                        #endregion

                    }
                case Designation.G:{
                        var associatesE = _salesman.GetAssociates(salesman.Code, (int)Designation.E);
                        int eligibleE = 0;
                        foreach (var associate in associatesE)
                        {
                            if (eligibleE == 4) { break; }
                            if (IsEligibleForOrdinalCommission(associate))
                            {
                                eligibleE++;
                            }
                        }
                        return eligibleE >= 4;
                    }
                case Designation.H:{
                        var associatesE = _salesman.GetAssociates(salesman.Code, (int)Designation.E);
                        int eligibleE = 0;
                        foreach (var associate in associatesE)
                        {
                            if (eligibleE == 6) { break; }
                            if (IsEligibleForOrdinalCommission(associate))
                            {
                                eligibleE++;
                            }
                        }
                        return eligibleE >= 6;
                    }
                case Designation.I:{
                        var associatesH = _salesman.GetAssociates(salesman.Code, (int)Designation.H);
                        int eligibleH = 0;
                        foreach (var associate in associatesH)
                        {
                            if (eligibleH == 6) { break; }
                            if (IsEligibleForOrdinalCommission(associate))
                            {
                                eligibleH++;
                            }
                        }
                        return eligibleH >= 6;
                    }
                case Designation.J:{
                        var associatesI = _salesman.GetAssociates(salesman.Code, (int)Designation.I);
                        int eligibleI = 0;
                        foreach (var associate in associatesI)
                        {
                            if (eligibleI == 3) { break; }
                            if (IsEligibleForOrdinalCommission(associate))
                            {
                                eligibleI++;
                            }
                        }
                        return eligibleI >= 3;
                    }
                case Designation.K:{
                        var associatesJ = _salesman.GetAssociates(salesman.Code, (int)Designation.J);
                        int eligibleJ = 0;
                        foreach (var associate in associatesJ)
                        {
                            if (eligibleJ == 2) { break; }
                            if (IsEligibleForOrdinalCommission(associate))
                            {
                                eligibleJ++;
                            }
                        }
                        return eligibleJ >= 2;
                    }
                default:
                    return false;
            }
        }

        private double SalesCommissionPercentage(double salesAmount)
        {
            return (salesAmount >= 1000 && salesAmount < 10000) ? 6
                   : (salesAmount >= 10000 && salesAmount < 300000) ? 7.5
                   : (salesAmount >= 300000 && salesAmount < 600000) ? 9
                   : (salesAmount >= 600000) ? 11
                   : 0;
        }
        public double SalesCommissionPercentage(Designation designation, double salesAmount)
        {
            switch (designation)
            {
                case  Designation.A:
                    return SalesCommissionPercentage(salesAmount);
                case  Designation.B:
                    return 12.5;
                case  Designation.C:
                    return 15;
                case  Designation.D:
                    return 17.5;
                case  Designation.E:
                    return 20;
                case  Designation.F:
                    return 21;
                case  Designation.G:
                    return 22;
                case  Designation.H:
                    return 23;
                case  Designation.I:
                    return 24;
                case  Designation.J:
                    return 25;
                case  Designation.K:
                default:
                    return 26;
            }
        }
        public double InboundCommissionPercentage(double salesAmount)
        {
            return salesAmount < 5000 ? 0
                        : (salesAmount >= 5000 && salesAmount < 50000) ? 5
                        : 7;
        }
        public double OutboundCommissionPercentage(double salesAmount)
        {
            return salesAmount < 5000 ? 0 : 5;
        }
        public double OrdinalCommissionPercentage(Designation designation)
        {
            switch (designation)
            {
                case  Designation.C:
                    return 1.5;
                case  Designation.D:
                    return 0.5;
                case  Designation.E:
                    return 6;
                case  Designation.F:
                    return 1.5;
                case  Designation.G:
                    return 1.5;
                case  Designation.H:
                    return 2.5;
                case  Designation.I:
                    return 1;
                case  Designation.J:
                    return 0.5;
                case  Designation.K:
                    return 1;
                default:
                    return 0;
            }
        }
        public double[] GbCommissionPercentage()
        {
            return new double[10] {2, 1.5, 1, 0.5, 0.5, 0.5, 0.5 ,0.5,0.25,0.25};
        }
        public  Designation NextDesignation(string code)
        {
            Dictionary< Designation,int> associates = _salesman.CountAssociates(code);
            if (associates[ Designation.J] >= 2)
            {
                return  Designation.K;
            }
            else if (associates[Designation.I] >= 3)
            {
                return Designation.J;
            }
            else if (associates[Designation.H] >= 6)
            {
                return Designation.I;
            }
            else if (associates[Designation.E] >= 6)
            {
                return Designation.H;
            }
            else if (associates[Designation.E] >= 4)
            {
                return Designation.G;
            }
            else if (associates[Designation.E] >= 2 
                && associates[Designation.C]>=1 
                && associates[Designation.B]>=2)
            {
                return Designation.F;
            }
            else if (associates[Designation.C] >= 3 
                && associates[Designation.B] >= 3)
            {
                return Designation.E;
            }
            else if (associates[Designation.C] >= 1 
                && associates[Designation.B] >= 2)
            {
                return Designation.D;
            }
            else if (associates[Designation.B] >= 3)
            {
                return Designation.C;
            }
            else if (associates[Designation.A] >= 2)
            {
                return Designation.B;
            }
            else
            {
                return Designation.A;
            }
        }
    }
}
