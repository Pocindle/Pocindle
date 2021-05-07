import axios from 'axios';

const siteUrl = 'http://localhost:61666';

const axiosConfig = axios.create({
  baseURL: `${siteUrl}/api`,
  headers: {
    Accept: 'application/json',
  },
});

export default axiosConfig;
