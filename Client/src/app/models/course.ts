import {Episode} from './episode';

export interface Course {
  id: number;
  name: string;
  price: number;
  description: string;
  photoFileName: string;
  instructor: string;
  isActive: boolean;
  waitingTimeBetweenEpisodes: number;
  visits: number;
  episodes: Episode[];
}
