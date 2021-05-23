import React, { useEffect } from 'react';
import { MainLayout } from '../../layouts';
import { retrieveArticles } from '../../api/apiRequests';
import { getJwtTokenFromLocalStorage } from '../../utils/localStorage';
import './mainPage.scss';

const MainPage: React.FC<{ onLogOut: () => void }> = ({ onLogOut }) => {
  useEffect(() => {
    const jwtToken = getJwtTokenFromLocalStorage();
    retrieveArticles(jwtToken).then((res) => console.log(res.data));
  }, []);

  return (
    <MainLayout onLogOut={onLogOut}>
      <div>asdasd</div>
    </MainLayout>
  );
};

export default MainPage;
