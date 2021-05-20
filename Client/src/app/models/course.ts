import {Episode} from './episode';
import {Category} from './category';

export interface Course {
  id: number;
  name: string;
  price: number;
  description: string;
  photoFileName: string;
  instructor: string;
  isActive: boolean;
  waitingTimeBetweenEpisodes: number;
  episodes: Episode[];
  categories: Category[];
}
