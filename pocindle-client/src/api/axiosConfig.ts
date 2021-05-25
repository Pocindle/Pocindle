import axios from 'axios';

const productionSiteUrl = 'https://pocindle.xyz';
const developSiteUrl = 'http://localhost:61666';

const siteUrl =
  process.env.NODE_ENV === 'production' ? productionSiteUrl : developSiteUrl;

const axiosConfig = axios.create({
  baseURL: `${siteUrl}/api`,
  headers: {
    Accept: 'application/json',
  },
});

export default axiosConfig;
