import { getJwtTokenExpirationDate } from './localStorage';

const validateExpirationDate = (onUnsuccessfulValidation: () => void): void => {
  const expirationDate = getJwtTokenExpirationDate();
  const currentDate = new Date();
  console.log(currentDate, expirationDate);
  if (currentDate > expirationDate) onUnsuccessfulValidation();
};

export const setExpirationDateInterval = (
  onExpiratedDate: () => void,
  intervalTime: number
): number => {
  return window.setInterval(
    () => validateExpirationDate(onExpiratedDate),
    intervalTime
  );
};
