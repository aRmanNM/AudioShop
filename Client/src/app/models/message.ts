export interface Message {
  id: number;
  title: string;
  body: string;
  link: string;
  courseId: number;
  userId: string;
  repeatAfterHour: number;
  isRepeatable: boolean;
  createdAt: Date;
  sendSMS: boolean;
  sendPush: boolean;
  sendInApp: boolean;
  messageType: number;
}
