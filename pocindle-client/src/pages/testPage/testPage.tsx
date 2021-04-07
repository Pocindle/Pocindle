import React from 'react';
import MainLayout from '../../layouts/mainLayout/mainLayout';
import { useRouteMatch, useLocation } from 'react-router-dom';
import { TestComponent } from '../../components';

const TestPage: React.FC = () => {
  const match = useRouteMatch();
  const location = useLocation();
  console.log('match', match);
  console.log('location', location);

  return (
    <React.Fragment>
      <MainLayout>
        Test page
        <TestComponent />
      </MainLayout>
    </React.Fragment>
  );
};

export default TestPage;
