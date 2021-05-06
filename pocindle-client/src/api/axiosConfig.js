import axios from 'axios';

const axiosConfig = axios.create({
  baseURL: 'https://pocindle.xyz/api',
  headers: {
    'content-type': 'multipart/form-data',
  },
});

export default axiosConfig;
