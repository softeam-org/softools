import React from 'react';

interface SidebarProps {
  categories: string[];
  onSelectCategory: (category: string) => void;
  selectedCategory: string;
}

const Sidebar: React.FC<SidebarProps> = ({ categories, onSelectCategory, selectedCategory }) => {
  return (
    <aside style={styles.sidebar}>
      {categories.map((category) => (
        <button
          key={category}
          style={{
            ...styles.button,
            backgroundColor: selectedCategory === category ? '#444' : 'transparent',
          }}
          onClick={() => onSelectCategory(category)}
        >
          {category}
        </button>
      ))}
    </aside>
  );
};

const styles: { [key: string]: React.CSSProperties } = {
  sidebar: {
    width: '200px',
    height: '100vh',
    backgroundColor: '#2d2d2d',
    color: '#fff',
    padding: '20px',
    boxSizing: 'border-box',
    display: 'flex',
    flexDirection: 'column',
    gap: '10px',
  },
  button: {
    padding: '10px',
    border: 'none',
    color: '#fff',
    textAlign: 'left',
    cursor: 'pointer',
  },
};

export default Sidebar;
