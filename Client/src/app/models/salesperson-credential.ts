import {Photo} from './photo';

export interface SalespersonCredential {
  id: number;
  userId: string;
  idCardNumber: string;
  bankAccountNumber: string;
  bankAccountShebaNumber: string;
  bankCardNumber: string;
  bankCardName: string;
  bankName: string;
  idCardPhoto: Photo;
  bankCardPhoto: Photo;
}
