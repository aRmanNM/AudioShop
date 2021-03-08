export interface Coupon {
  id: number;
  discountPercentage: number;
  description: string;
  code: string;
  isActive: boolean;
  userId: string;
}

