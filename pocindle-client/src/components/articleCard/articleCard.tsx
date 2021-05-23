import React from 'react';
import './articleCard.scss';

interface article {
  url: string;
  title: string;
  excerpt?: string;
  listenDurationEstimate?: number;
  wordCount?: number;
}

export type { article };

const ArticleCard: React.FC<article> = ({
  url,
  title,
  excerpt,
  listenDurationEstimate,
  wordCount,
}) => {
  return (
    <div className="article-card">
      <div className="article-card__wrapper">
        <div className="article-card__item">{title}</div>
        <div className="article-card__item">{url}</div>
        <div className="article-card__item">{excerpt}</div>
        <div className="article-card__item">{listenDurationEstimate}</div>
        <div className="article-card__item">{wordCount}</div>
      </div>
    </div>
  );
};

export default ArticleCard;
