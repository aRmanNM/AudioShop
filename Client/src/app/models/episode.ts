import {Audio} from './audio';

export interface Episode {
  id: number;
  name: string;
  description: string;
  sort: number;
  price: number;
  audios: Audio[];
  courseId: number;
}
