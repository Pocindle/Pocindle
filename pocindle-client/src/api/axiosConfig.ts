import axios from 'axios';

const siteUrl =
  process.env.NODE_ENV === 'production'
    ? process.env.REACT_APP_PRODUCTION_SITE_URL
    : process.env.REACT_APP_DEVELOP_SITE_URL;

const axiosConfig = axios.create({
  baseURL: `${siteUrl}/api`,
  headers: {
    Accept: 'application/json',
  },
});

export default axiosConfig;
