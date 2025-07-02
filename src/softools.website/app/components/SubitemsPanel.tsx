interface Subitem {
    name: string;
    link: string;
  }
  
  interface SubitemsPanelProps {
    items: Subitem[];
    onSelect: (link: string) => void;
  }
  
  const SubitemsPanel: React.FC<SubitemsPanelProps> = ({ items, onSelect }) => (
    <div className="w-48 bg-[var(--softeam1)] p-4 space-y-2 shadow-xl shadow-black z-5">
      {items.map(({ name, link }) => (
        <button
          key={link}
          onClick={() => onSelect(link)}
          className="w-full text-left px-3 py-2 rounded bg-[var(--softeam1)] hover:bg-[var(--softeam2)] text-sm"
        >
          {name}
        </button>
      ))}
    </div>
  );
  
  export default SubitemsPanel;
  