<template>
  <div id="app">
    <router-view v-slot="{ Component }">
      <transition name="route" mode="out-in">
        <component :is="Component" />
      </transition>
    </router-view>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

export default defineComponent({
  name: 'App'
});
</script>

<style lang="scss">
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
}
:root {
  --color-ui-background: theme('colors.gray.100');
  --color-ui-login-background: theme('colors.gray.300');
  --color-ui-typo: theme('colors.gray.700');
  --color-ui-sidebar: theme('colors.gray.200');
  --color-ui-border: theme('colors.gray.300');
  --color-ui-primary: theme('colors.indigo.600');
}
/*Scrollbar*/
/* width */
::-webkit-scrollbar {
  width: 12px;
}
/* Router view transitions */
.route-enter-from,
.route-leave-to {
  opacity: 0;
  transform: translateY(-30px);
}

.route-enter-active,
.route-leave-active {
  transition: all 0.2s ease;
}

.route-enter-to,
.route-leave-from {
  opacity: 1;
  transform: translateY(0);
}

/* Track */
::-webkit-scrollbar-track {
  border-radius: 100vh;
  background: var(--color-ui-sidebar);
}
/* Handle */
::-webkit-scrollbar-thumb {
  background: var(--color-ui-border);
  border-radius: 100vh;
  border: 3px solid #edf2f7;
}
/* Handle on hover */
::-webkit-scrollbar-thumb:hover {
  background: #c1cacb;
}

html[lights-out] {
  --color-ui-background: theme('colors.gray.900');
  --color-ui-typo: theme('colors.gray.100');
  --color-ui-sidebar: theme('colors.gray.800');
  --color-ui-border: theme('colors.gray.800');
  --color-ui-primary: theme('colors.indigo.500');

  pre[class*='language-'],
  code[class*='language-'] {
    @apply bg-ui-border;
  }
}

* {
  transition-property: color, background-color, border-color;
  transition-duration: 200ms;
  transition-timing-function: ease-in-out;
}

h1,
h2,
h3,
h4 {
  @apply leading-snug font-black mb-4 text-ui-typo;

  &:hover {
    a::before {
      @apply opacity-100;
    }
  }

  a {
    &::before {
      content: '#';
      margin-left: -1em;
      padding-right: 1em;
      @apply text-ui-primary absolute opacity-0 float-left;
    }
  }
}

h1 {
  @apply text-4xl;
}

h2 {
  @apply text-2xl;
}

h3 {
  @apply text-xl;
}

h4 {
  @apply text-lg;
}

p,
ol,
ul,
pre,
strong,
blockquote {
  @apply mb-4 text-base text-ui-typo;
}

.content {
  a {
    @apply text-ui-primary underline;
  }

  h1,
  h2,
  h3,
  h4,
  h5,
  h6 {
    @apply -mt-12 pt-20;
  }

  h2 + h3,
  h2 + h2,
  h3 + h3 {
    @apply border-none -mt-20;
  }

  h2,
  h3 {
    @apply border-b border-ui-border pb-1 mb-3;
  }

  ul {
    @apply list-disc;

    ul {
      list-style: circle;
    }
  }

  ol {
    @apply list-decimal;
  }

  ol,
  ul {
    @apply pl-5 py-1;

    li {
      @apply mb-2;

      p {
        @apply mb-0;
      }

      &:last-child {
        @apply mb-0;
      }
    }
  }
}

blockquote {
  @apply border-l-4 border-ui-border py-2 pl-4;

  p:last-child {
    @apply mb-0;
  }
}

code {
  @apply px-1 py-1 text-ui-typo bg-ui-sidebar font-mono border-b border-r border-ui-border text-sm rounded;
}

pre[class*='language-'] {
  @apply max-w-full overflow-x-auto rounded;

  & + p {
    @apply mt-4;
  }

  & > code[class*='language-'] {
    @apply border-none leading-relaxed;
  }
}

header {
  background-color: rgba(255, 255, 255, 0.9);
  backdrop-filter: blur(4px);
}

table {
  @apply text-left mb-6;

  th {
    @apply py-3 px-4 bg-gray-400 border-white uppercase text-sm text-gray-600 font-bold tracking-wider;
  }

  td {
    @apply px-4 py-2 border-white border-b text-sm text-gray-600 font-semibold tracking-wider;
  }

  tr {
    @apply border-b border-ui-border;
    &:nth-child(even) {
      @apply bg-gray-200;
    }
    &:nth-child(odd) {
      @apply bg-white;
    }
  }
}

.sidebar {
  overflow: auto;
  @apply fixed bg-ui-background px-4 inset-x-0 bottom-0 w-full border-r border-ui-border transition-all z-40;
  transform: translateX(-100%);

  &.open {
    transform: translateX(0);
  }

  @screen md {
    @apply w-1/4 px-0 bg-transparent top-0 bottom-auto inset-x-auto sticky z-0;
    transform: translateX(0);
  }
}
</style>
