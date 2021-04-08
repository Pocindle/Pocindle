import React, { useContext } from 'react';
import MainLayout from '../../layouts/mainLayout/mainLayout';
import { useRouteMatch, useLocation } from 'react-router-dom';
import { TestComponent } from '../../components';
import { ThemeContext, LanguageContext } from '../../providers';

const TestPage: React.FC = () => {
  const match = useRouteMatch();
  const location = useLocation();
  console.log('match', match);
  console.log('location', location);

  const { theme, switchTheme } = useContext(ThemeContext);
  const { language, switchLanguage } = useContext(LanguageContext);

  return (
    <React.Fragment>
      <MainLayout>
        Test page
        <TestComponent />
        <div>
          {`Theme: ${theme.name}`}
          <button onClick={() => switchTheme('light')}>light</button>
          <button onClick={() => switchTheme('dark')}>dark</button>
        </div>
        <div>
          {`Language: ${language.name}`}
          <button onClick={() => switchLanguage('ru')}>Русский</button>
          <button onClick={() => switchLanguage('eng')}>English</button>
        </div>
      </MainLayout>
    </React.Fragment>
  );
};

export default TestPage;
