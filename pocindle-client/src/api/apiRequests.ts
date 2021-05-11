import axios from './axiosConfig';
import { RequestDto } from './dto/requestDto';

export const postAuthRequest = async () => {
  const data = await axios.post<RequestDto>('/auth/request');
  console.log(data, 'asd');
  return data;
};

export const postRequestToken = (
  requestToken: string,
  callbackOnSuccess: () => void,
  callbackOnError: () => void
): Promise<void> => {
  return axios
    .post(`auth/authorize/${requestToken}`)
    .then((result) => {
      console.log(result.data);
      callbackOnSuccess();
    })
    .catch((error) => {
      console.log(error.message);
      callbackOnError();
    });
};
