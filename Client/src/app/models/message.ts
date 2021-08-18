export interface Message {
  id: number;
  title: string;
  body: string;
  link: string;
  courseId: number;
  userId: string;
  clockRangeBegin: number;
  clockRangeEnd: number;
  isRepeatable: boolean;
  createdAt: Date;
  sendSMS: boolean;
  sendPush: boolean;
  messageType: number;
  isSeen: boolean;
}
