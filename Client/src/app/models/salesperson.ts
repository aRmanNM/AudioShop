import {Credential} from './credential';

export interface Salesperson {
  id: string;
  userName: string;
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
  discountPercentageOfSalesperson: number;
  salespersonCredential: Credential;
  credentialAccepted: boolean;
}
