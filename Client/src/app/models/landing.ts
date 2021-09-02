import {Photo} from './photo';
import {ContentFile} from './content-file';

export interface Landing {
  id: number;
  description: string;
  urlName: string;
  logo: Photo;
  logoEnabled: boolean;
  title: string;
  titleEnabled: boolean;
  media: ContentFile;
  mediaEnabled: boolean;
  text1: string;
  text1Enabled: boolean;
  button: string;
  buttonLink: string;
  buttonEnabled: boolean;
  buttonClickCount: number;
  text2: string;
  text2Enabled: boolean;
  gift: string;
  giftEnabled: string;
  phoneBoxEnabled: boolean;
  phoneNumberCounts: number;
  buttonsColor: string;
  background: Photo;
}
