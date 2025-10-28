// Admin UI interactions: sidebar collapse, dark-mode toggle, top-right menu
(function(){
  const sidebar = document.querySelector('.sidebar');
  const sidebarToggle = document.querySelector('.SidebarOpener') || document.querySelector('.bx-menu');
  const themeToggle = document.getElementById('theme-toggle');
  const menuBtn = document.querySelector('.menu');
  const dropdown = document.querySelector('.dropdown-menu');

  // Initialize sidebar state from localStorage
  try{
    const closed = localStorage.getItem('admin.sidebar.closed');
    if(closed === 'true') sidebar.classList.add('close'); else sidebar.classList.remove('close');
  } catch(e){}

  if(sidebarToggle){
    sidebarToggle.addEventListener('click', (e)=>{
      e.preventDefault();
      if(sidebar) sidebar.classList.toggle('close');
      try{ localStorage.setItem('admin.sidebar.closed', sidebar.classList.contains('close')) }catch(e){}
    });
  }

  // Dark mode toggle: read/write localStorage and toggle body.dark
  function applyDark(dark){
    if(dark) document.body.classList.add('dark'); else document.body.classList.remove('dark');
    try{ localStorage.setItem('admin.darkmode', dark ? 'true' : 'false'); }catch(e){}
  }

  // initialize from stored value or checkbox state
  try{
    const stored = localStorage.getItem('admin.darkmode');
    if(stored !== null) applyDark(stored === 'true');
    else if(themeToggle && themeToggle.checked) applyDark(true);
  }catch(e){}

  if(themeToggle){
    themeToggle.addEventListener('change', ()=>{
      applyDark(themeToggle.checked);
    });
  }

  // Top-right menu simple toggle (non-bootstrap fallback)
  if(menuBtn){
    menuBtn.addEventListener('click', function(e){
      e.preventDefault();
      const menu = this.closest('nav').querySelector('.dropdown-menu');
      if(!menu) return;
      menu.classList.toggle('show');
    });

    // close when clicking outside
    document.addEventListener('click', function(ev){
      const menu = document.querySelector('.dropdown-menu');
      if(!menu) return;
      if(ev.target.closest('.menu')) return; // clicked the button
      if(menu.classList.contains('show')) menu.classList.remove('show');
    });
  }

})();
