import axios from './axiosConfig';
import { RequestDto } from './dto/requestDto';

export const postAuthRequest = async () => {
  const data: RequestDto = await axios.post('/auth/request');
  console.log(data, 'asd');
  return data;
};
