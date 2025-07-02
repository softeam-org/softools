'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import TopBar from './components/Topbar';
import IconSidebar from './components/IconSidebar';
import SubitemsPanel from './components/SubitemsPanel';

const categoryData = {
  Files: [
    { name: 'Upload Documents', link: '/files/upload' },
    { name: 'View Reports', link: '/files/reports' },
    { name: 'Archive', link: '/files/archive' },
  ],
  Members: [
    { name: 'Add Member', link: '/members/add' },
    { name: 'Manage Roles', link: '/members/roles' },
    { name: 'Attendance', link: '/members/attendance' },
  ],
  Settings: [
    { name: 'Profile Settings', link: '/settings/profile' },
    { name: 'Access Control', link: '/settings/access' },
    { name: 'Notifications', link: '/settings/notifications' },
  ],
};

export default function Home() {
  const [selectedCategory, setSelectedCategory] = useState(Object.keys(categoryData)[0]);
  const router = useRouter();

  const handleSubitemClick = (link: string) => {
    console.log('Navigate to:', link);
    router.push(link); // enable navigation when ready
  };

  return (
    <div className="flex flex-col h-screen">
      <TopBar title="Softools" />
      <div className="flex flex-1">
        <IconSidebar
          categories={Object.keys(categoryData)}
          selected={selectedCategory}
          onSelect={setSelectedCategory}
        />
        <SubitemsPanel
          items={categoryData[selectedCategory] ?? []}  // <-- fallback here
          onSelect={handleSubitemClick}
        />

        <main className="flex-1 p-8">
          <h1 className="text-4xl font-semibold subtitle">Projetos</h1>
          <p className="mt-4 text-gray-700 dark:text-gray-300">
            Lorem ipsum dolor sit amet consectetur adipisicing elit. Enim vitae, libero odit ullam quod modi consequatur commodi sequi a eveniet veritatis harum, facilis quos fuga obcaecati recusandae voluptatibus omnis veniam?
          </p>
          <h1 className="text-4xl font-semibold subtitle">Recursos Humanos</h1>
          <p className="mt-4 text-gray-700 dark:text-gray-300">
            Lorem ipsum dolor sit amet consectetur adipisicing elit. Enim vitae, libero odit ullam quod modi consequatur commodi sequi a eveniet veritatis harum, facilis quos fuga obcaecati recusandae voluptatibus omnis veniam?
          </p>
        </main>
      </div>
    </div>
  );
}
