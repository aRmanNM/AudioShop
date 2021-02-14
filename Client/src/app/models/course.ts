import {Episode} from './episode';

export interface Course {
  id: number;
  name: string;
  price: number;
  description: string;
  photoFileName: string;
  episodes: Episode[];
}
