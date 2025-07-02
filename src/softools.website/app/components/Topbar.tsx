import React from 'react';
import Image from 'next/image';

interface TopBarProps {
  title: string;
}

const TopBar: React.FC<TopBarProps> = ({ title }) => {
  return (
    <header style={styles.header} className="title text-4xl">
      <Image src="/softeam.png" alt="softeam" width={200} height={50} />
      <h1 style={styles.title}>{title}</h1>
    </header>
  );
};

const styles: { [key: string]: React.CSSProperties } = {
  header: {
    backgroundColor: 'var(--softeam1)',
    color: '#fff',
    padding: '10px 20px',
    display: 'flex',
    alignItems: 'center',
    boxShadow: '0 2px 4px rgba(0,0,0,0.1)',
  },
  title: {
    margin: 0,
    height: '50px',
    display: 'flex',
    alignItems: 'center',
    paddingLeft: '10px', // spacing between image and text
  },
};

export default TopBar;
