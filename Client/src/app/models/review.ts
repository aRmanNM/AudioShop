export interface Review {
  id: number;
  text: string;
  rating: number;
  accepted: boolean;
  date: Date;
  courseId: number;
  courseName: string;
  userId: string;
  userFirstName: string;
  userLastName: string;
}
