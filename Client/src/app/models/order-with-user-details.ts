export interface OrderWithUserDetails {
  id: number;
  userName: string;
  totalPrice: number;
  discount: number;
  priceToPay: number;
  status: boolean;
  paymentReceipt: string;
  date: Date;
  salespersonCouponCode: string;
  otherCouponCode: string;
  memberName: string;
}
