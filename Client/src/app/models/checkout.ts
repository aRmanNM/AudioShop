export interface  Checkout {
  id: number;
  userId: string;
  status: boolean;
  amountToCheckout: number;
  paymentReceipt: string;
  createdAt: Date;
  paidAt: Date;
}

