import React, { useContext } from 'react';
import MainLayout from '../../layouts/mainLayout/mainLayout';
import { useRouteMatch, useLocation } from 'react-router-dom';
import { TestComponent } from '../../components';
import { ThemeContext } from '../../providers';

const TestPage: React.FC = () => {
  const match = useRouteMatch();
  const location = useLocation();
  console.log('match', match);
  console.log('location', location);

  const { theme, switchTheme } = useContext(ThemeContext);

  return (
    <React.Fragment>
      <MainLayout>
        Test page
        <TestComponent />
        <div>
          {`Theme: ${theme.test}`}
          <button onClick={() => switchTheme('light')}>light</button>
          <button onClick={() => switchTheme('dark')}>dark</button>
        </div>
      </MainLayout>
    </React.Fragment>
  );
};

export default TestPage;
