import {Episode} from './episode';

export interface Course {
  id: number;
  name: string;
  price: number;
  description: string;
  photoFileName: string;
  isActive: boolean;
  waitingTimeBetweenEpisodes: number;
  episodes: Episode[];
}
