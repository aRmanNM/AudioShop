import {Photo} from './photo';

export interface Credential {
  id: number;
  userId: string;
  idCardNumber: string;
  bankAccountNumber: string;
  bankAccountShebaNumber: string;
  bankCardNumber: string;
  bankCardName: string;
  bankName: string;
  phone: number;
  idCardPhoto: Photo;
  bankCardPhoto: Photo;
}
