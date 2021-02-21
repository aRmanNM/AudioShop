export interface  Checkout {
  id: number;
  userId: string;
  userName: string;
  status: boolean;
  amountToCheckout: number;
  paymentReceipt: string;
  createdAt: Date;
  paidAt: Date;
}

