export interface Order {
  id: number;
  userId: string;
  totalPrice: number;
  discountedPrice: number;
  priceToPay: number;
  couponCode: string;
  status: boolean;
  paymentReceipt: string;
  date: Date;
  salespersonShareAmount: number;
  salespersonSharePaid: boolean;
}
