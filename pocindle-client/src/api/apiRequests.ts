import axios from './axiosConfig';
import { AxiosResponse } from 'axios';
import { RequestDto, JwtTokenDto, PocketRetrieveDto } from './dto';

export const postAuthRequest = async (): Promise<AxiosResponse<RequestDto>> => {
  const data = await axios.post<RequestDto>('/auth/request');
  console.log(data, 'asd');
  return data;
};

export const postRequestToken = (
  requestToken: string,
  callbackOnSuccess: (jwtToken: string) => void,
  callbackOnError: () => void
): Promise<void> => {
  return axios
    .post<JwtTokenDto>(`auth/authorize/${requestToken}`)
    .then((result) => {
      console.log(result.data);
      callbackOnSuccess(result.data.jwtToken);
    })
    .catch((error) => {
      console.log(error.message);
      callbackOnError();
    });
};

export const retrieveArticles = (
  jwtToken: string
): Promise<AxiosResponse<PocketRetrieveDto>> => {
  console.log(jwtToken);
  return axios.get<PocketRetrieveDto>('/pocket/retrieveAll', {
    headers: {
      Authorization: `Bearer ${jwtToken}`,
    },
  });
};
