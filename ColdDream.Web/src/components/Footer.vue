<template>
  <footer class="bg-white border-t border-gray-200 mt-auto">
    <div class="max-w-7xl mx-auto py-8 px-4 sm:px-6 lg:px-8">
      <div class="grid grid-cols-1 md:grid-cols-3 gap-8">
        <div>
          <h3 class="text-sm font-semibold text-gray-400 tracking-wider uppercase">关于我们</h3>
          <p class="mt-4 text-base text-gray-500">
            ColdDream 致力于为您提供最极致的旅行体验。从冰雪奇缘到热带风情，我们用心策划每一条路线。
          </p>
        </div>
        <div>
          <h3 class="text-sm font-semibold text-gray-400 tracking-wider uppercase">联系方式</h3>
          <ul class="mt-4 space-y-4">
            <li class="text-base text-gray-500">电话: 400-888-9999</li>
            <li class="text-base text-gray-500">邮箱: support@colddream.com</li>
            <li class="text-base text-gray-500">地址: 某某市某某区科技园</li>
          </ul>
        </div>
        <div>
          <h3 class="text-sm font-semibold text-gray-400 tracking-wider uppercase">关注我们</h3>
          <div class="mt-4 flex space-x-6">
            <a href="#" class="text-gray-400 hover:text-gray-500">
              <span class="sr-only">微信</span>
              <!-- Wechat Icon -->
              <svg class="h-6 w-6" fill="currentColor" viewBox="0 0 24 24"><path d="M8.5,15c-4.1,0-7.5-3-7.5-6.8S4.3,1.5,8.5,1.5S16,4.5,16,8.2S12.6,15,8.5,15z M8.5,15c0.5,0,0.9,0,1.4-0.1 c-0.5,2.4-2.8,4.1-5.2,3.8c-1.2-0.1-2.2-0.7-3-1.6c0.4,0,0.9,0.1,1.3,0.1C5,17.2,6.7,16.4,8,15.1C8.2,15,8.3,15,8.5,15z M17,17.5 c-3,0-5.5-2-5.5-4.5s2.5-4.5,5.5-4.5s5.5,2,5.5,4.5S20,17.5,17,17.5z"/></svg>
            </a>
          </div>
        </div>
      </div>
      <div class="mt-8 border-t border-gray-200 pt-8 text-center">
        <p class="text-base text-gray-400">&copy; {{ new Date().getFullYear() }} ColdDream 冷梦旅行. All rights reserved.</p>
      </div>
    </div>
  </footer>
</template>
