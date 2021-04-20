import {Salesperson} from './salesperson';
import {Photo} from './photo';

export interface  Checkout {
  id: number;
  userId: string;
  userName: string;
  status: boolean;
  amountToCheckout: number;
  paymentReceipt: string;
  createdAt: Date;
  paidAt: Date;
  user: Salesperson;
  receiptPhoto: Photo;
}

