import {ContentFile} from './content-file';
import {Place} from './place';

export interface Ad {
  id: number;
  title: string;
  description: string;
  link: string;
  file: ContentFile;
  adType: number;
  places: Place[];
  isEnabled: boolean;
}
