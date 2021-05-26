import axios from './axiosConfig';
import { AxiosResponse } from 'axios';
import {
  RequestDto,
  JwtTokenDto,
  PocketRetrieveDto,
  UserDto,
  DeliveryDto,
  ServerEmailDto,
} from './dto';

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
      console.log(error);
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

export const retrieveUserInfo = (
  jwtToken: string
): Promise<AxiosResponse<UserDto>> => {
  return axios.get<UserDto>('user/', {
    headers: {
      Authorization: `Bearer ${jwtToken}`,
    },
  });
};

export const setKindleEmailAddress = (
  jwtToken: string,
  kindleEmail: string
): Promise<AxiosResponse<null>> => {
  return axios.post<null>(
    `user/kindle-email/${kindleEmail}`,
    {},
    {
      headers: {
        Authorization: `Bearer ${jwtToken}`,
      },
    }
  );
};

export const sendArticleForConvertation = (
  jwtToken: string,
  articleUrl: string
): Promise<AxiosResponse<DeliveryDto>> => {
  return axios.post<DeliveryDto>(
    `/convert/${encodeURIComponent(articleUrl)}`,
    {},
    {
      headers: {
        Authorization: `Bearer ${jwtToken}`,
      },
    }
  );
};

export const deliverArticle = (
  jwtToken: string,
  deliveryId: number
): Promise<AxiosResponse<DeliveryDto>> => {
  return axios.post<DeliveryDto>(
    `delivery/send/${deliveryId}`,
    {},
    {
      headers: {
        Authorization: `Bearer ${jwtToken}`,
      },
    }
  );
};

export const retrieveServerEmailAddress = (
  jwtToken: string
): Promise<AxiosResponse<ServerEmailDto>> => {
  return axios.get<ServerEmailDto>('delivery/server-email-address', {
    headers: {
      Authorization: `Bearer ${jwtToken}`,
    },
  });
};
