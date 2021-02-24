import {SalespersonCredential} from './salesperson-credential';


export interface SalespersonInfo {
  firstName: string;
  lastName: string;
  city: string;
  country: string;
  age: number;
  gender: string;
  salePercentageOfSalesperson: number;
  currentSalesOfSalesperson: number;
  totalSalesOfSalesperson: number;
  couponCode: string;
  salespersonCredential: SalespersonCredential;
  credentialAccepted: boolean;
}
